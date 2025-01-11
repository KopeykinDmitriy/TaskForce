import React, { useState } from "react";
import { useParams } from "react-router-dom";
import { fetchTasksData, train } from "../services/api";

const TrainModelPage = () => {
  const { projectId } = useParams();
  const [isTraining, setIsTraining] = useState(false);
  const [progress, setProgress] = useState([]);
  const [error, setError] = useState("");

  const handleTrainModel = async () => {
    setIsTraining(true);
    setProgress([]);
    setError("");

    try {
      const tasksData = await fetchTasksData(projectId);

      await train(tasksData);

      alert('Модель успешно обучена!');
    } catch (err) {
      setError(err.message || "Произошла ошибка.");
    } finally {
      setIsTraining(false);
    }
  };

  return (
    <div style={{ padding: "20%", left: "10%", width: "90%" }}>
      <h1>Обучение модели</h1>
      <div style={{ marginBottom: "20px" }}>
        <button
          onClick={handleTrainModel}
          disabled={isTraining}
          style={{
            marginLeft: "10px",
            padding: "10px 20px",
            backgroundColor: isTraining ? "#ccc" : "#007bff",
            color: "#fff",
            border: "none",
            cursor: isTraining ? "not-allowed" : "pointer",
          }}
        >
          {isTraining ? "Обучение..." : "Начать обучение"}
        </button>
      </div>

      {error && <p style={{ color: "red" }}>{error}</p>}

      {progress.length > 0 && (
        <div>
          <h3>Прогресс:</h3>
          <ul>
            {progress.map((item, index) => (
              <li key={index}>
                Эпоха: {item.epoch}, Loss: {item.loss.toFixed(2)}, MAE:{" "}
                {item.mae.toFixed(2)}, Val Loss: {item.val_loss.toFixed(2)}, Val
                MAE: {item.val_mae.toFixed(2)}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default TrainModelPage;
