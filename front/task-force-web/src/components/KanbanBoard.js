import React, { useState, useEffect } from 'react';
import { fetchTasks } from '../services/api'; // Подключаем метод для получения задач
import TaskCard from './TaskCard';
import { Link } from 'react-router-dom';
import CreateTaskModal from './CreateTaskModal';

const KanbanBoard = () => {
  const [tasks, setTasks] = useState([]);
  const [showModal, setShowModal] = useState(false); // Для показа модального окна

  const handleShow = () => setShowModal(true); // Открыть модальное окно
  const handleClose = () => setShowModal(false); // Закрыть модальное окно

  useEffect(() => {
    // Получаем задачи при загрузке страницы
    const getTasks = async () => {
      const fetchedTasks = await fetchTasks();
      setTasks(fetchedTasks);
    };

    getTasks();
  }, []);

  // Функция для отображения задач по статусам
  const getTasksByStatus = (status) => {
    return tasks.filter(task => task.status === status);
  };

  return (
    <div className="container mt-4">
      <div className="row">
        <div className="col-md-4">
          <h2 className="text-center">To Do</h2>
          {getTasksByStatus('to_do').map(task => (
            <TaskCard key={task.id} task={task} />
          ))}
        </div>
        <div className="col-md-4">
          <h2 className="text-center">In Progress</h2>
          {getTasksByStatus('in_progress').map(task => (
            <TaskCard key={task.id} task={task} />
          ))}
        </div>
        <div className="col-md-4">
          <h2 className="text-center">Done</h2>
          {getTasksByStatus('done').map(task => (
            <TaskCard key={task.id} task={task} />
          ))}
        </div>
      </div>
      {/* Кнопка для создания новой задачи */}
      <button className="btn btn-primary mt-3" onClick={handleShow}>
        Создать задачу
      </button>
      
      {/* Модальное окно */}
      <CreateTaskModal showModal={showModal} handleClose={handleClose} />
    </div>
  );
};

export default KanbanBoard;
