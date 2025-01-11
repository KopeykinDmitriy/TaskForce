import numpy as np
import pandas as pd
import tensorflow as tf
from flask import Flask, request, jsonify, Response
from tensorflow.keras.models import load_model
from tensorflow.keras.layers import TextVectorization
from keras.losses import MeanSquaredError
from sklearn.model_selection import train_test_split
import os
import requests

app = Flask(__name__)

EMBEDDING_DIM = 300
MAX_TOKENS = 10000
MAX_LENGTH = 150
MODEL_PATH = "task_duration_model_advanced.h5"
VECTORIZER_PATH = "text_vectorizer_layer_advanced.keras"
GLOVE_PATH = "ft_native_300_ru_wiki_lenta_lemmatize.vec"


def load_glove_embeddings(filepath, embedding_dim):
    embeddings_index = {}
    with open(filepath, encoding="utf-8") as f:
        for line in f:
            values = line.split()
            word = " ".join(values[:-embedding_dim])
            coefficients = np.asarray(values[-embedding_dim:], dtype='float32')
            embeddings_index[word] = coefficients
    print(f"Загружено {len(embeddings_index)} эмбеддингов.")
    return embeddings_index


def create_embedding_matrix(word_index, embeddings_index, embedding_dim):
    embedding_matrix = np.zeros((len(word_index), embedding_dim))
    for word, i in word_index.items():
        embedding_vector = embeddings_index.get(word)
        if embedding_vector is not None:
            embedding_matrix[i] = embedding_vector
    return embedding_matrix

@app.route('/train', methods=['POST'])
def train():
    file = request.files.get('file')
    if not file:
        return jsonify({"error": "Файл не предоставлен."}), 400

    tasks_data = pd.read_excel(file)
    tasks_data['Срок_выполнения'] = (pd.to_datetime(tasks_data['Дата конца']) - pd.to_datetime(tasks_data['Дата начала'])).dt.days
    tasks_data = tasks_data[tasks_data['Срок_выполнения'] > 0]

    texts = [f"{t} {d}" for t, d in zip(tasks_data['Название'], tasks_data['Описание'])]
    durations = tasks_data['Срок_выполнения']

    X_train_texts, X_test_texts, y_train, y_test = train_test_split(texts, durations, test_size=0.2, random_state=20)

    vectorizer = TextVectorization(max_tokens=MAX_TOKENS, output_sequence_length=MAX_LENGTH, ngrams=3)
    vectorizer.adapt(X_train_texts)

    X_train = vectorizer(X_train_texts).numpy()
    X_test = vectorizer(X_test_texts).numpy()

    embeddings_index = load_glove_embeddings(GLOVE_PATH, EMBEDDING_DIM)
    word_index = vectorizer.get_vocabulary()
    word_to_index = {word: i for i, word in enumerate(word_index)}
    embedding_matrix = create_embedding_matrix(word_to_index, embeddings_index, EMBEDDING_DIM)

    model = tf.keras.Sequential([
        tf.keras.layers.Embedding(input_dim=len(word_index), output_dim=EMBEDDING_DIM, 
                                  weights=[embedding_matrix], input_length=MAX_LENGTH, trainable=False),
        tf.keras.layers.Bidirectional(tf.keras.layers.LSTM(128, dropout=0.2)),
        tf.keras.layers.Dense(64, activation='relu'),
        tf.keras.layers.Dropout(0.2),
        tf.keras.layers.Dense(16, activation='relu'),
        tf.keras.layers.Dense(1, activation='linear')
    ])

    model.compile(optimizer=tf.keras.optimizers.AdamW(), loss='mse', metrics=['mae'])

    model.fit(X_train, y_train, epochs=5, batch_size=32, validation_data=(X_test, y_test))

    model.save(MODEL_PATH)
    tf.keras.models.save_model(vectorizer, VECTORIZER_PATH)

    return jsonify({"message": "Модель успешно обучена и сохранена."})

@app.route('/predict', methods=['POST'])
def predict():
    data = request.get_json()
    if not data:
        return jsonify({"error": "Данные не предоставлены."}), 400

    task_name = data.get("task_name", "")
    tags = data.get("tags", "")
    description = data.get("description", "")

    if not task_name or not description:
        return jsonify({"error": "Недостаточно данных для предсказания."}), 400

    if not os.path.exists(MODEL_PATH) or not os.path.exists(VECTORIZER_PATH):
        return jsonify({"error": "Модель или векторизатор не найдены. Пожалуйста, обучите модель."}), 500

    tf.keras.utils.get_custom_objects()['mse'] = MeanSquaredError()
    model = load_model(MODEL_PATH)
    vectorizer = load_model(VECTORIZER_PATH)

    input_text = f"{task_name} {description} {tags}"
    input_vector = vectorizer(tf.constant([input_text]))
    prediction = model.predict(input_vector)[0][0]

    return jsonify({"predicted_days": round(float(prediction), 2)})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
