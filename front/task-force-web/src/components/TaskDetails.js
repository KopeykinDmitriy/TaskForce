import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { fetchTaskById, updateTask } from '../services/api'; // Методы для получения и обновления задачи

const TaskDetails = () => {
  const { taskId } = useParams(); // Получаем ID задачи из URL
  const navigate = useNavigate();
  const [task, setTask] = useState(null);
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [status, setStatus] = useState('');

  useEffect(() => {
    const getTask = async () => {
      const fetchedTask = await fetchTaskById(taskId);
      setTask(fetchedTask);
      setTitle(fetchedTask.title);
      setDescription(fetchedTask.description);
      setStatus(fetchedTask.status)
    };

    getTask();
  }, [taskId]);

  const handleSave = async () => {
    await updateTask(taskId, { title, description, status });
    navigate('/'); // Возвращаемся на главную страницу
  };

  if (!task) {
    return <p>Загрузка...</p>;
  }

  return (
    <div className="task-details">
      <h2>Редактирование задачи</h2>
      <input
        type="text"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        placeholder="Название задачи"
      />
      <textarea
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        placeholder="Описание задачи"
      />
      <textarea
        value={status}
        onChange={(e) => setStatus(e.target.value)}
        placeholder="Статус задачи"
      />
      <button onClick={handleSave}>Сохранить изменения</button>
    </div>
  );
};

export default TaskDetails;
