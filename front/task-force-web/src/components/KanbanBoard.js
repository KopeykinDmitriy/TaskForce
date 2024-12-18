import React, { useState, useEffect } from 'react';
import { fetchTasks } from '../services/api'; // Подключаем метод для получения задач
import { useParams } from 'react-router-dom';
import TaskCard from './TaskCard';
import '../styles/Kanban.css'

const KanbanBoard = () => {
  const [tasks, setTasks] = useState([]);

  const { projectId } = useParams();

  useEffect(() => {
    // Получаем задачи при загрузке страницы
    const getTasks = async () => {
      const fetchedTasks = await fetchTasks(projectId);
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
      <div className="row justify-content-between">
        <div className="col-md-4">
          <h2 class="text-center kanban-status">To Do</h2>
          <div className='p-3' style={{backgroundColor:"rgb(221, 221, 221)", height:"100%"}}>
            {getTasksByStatus('TO_DO').map(task => (
              <TaskCard key={task.id} task={task} projectId={projectId} />
            ))}
          </div>
        </div>
        <div className="col-md-4">
          <h2 className="text-center kanban-status">In Progress</h2>
          <div className='p-3' style={{backgroundColor:"rgb(221, 221, 221)", height:"100%"}}>
          {getTasksByStatus('IN_PROGRESS').map(task => (
            <TaskCard key={task.id} task={task} projectId={projectId} />
          ))}
          </div>
        </div>
        <div className="col-md-4">
          <h2 className="text-center kanban-status">Done</h2>
          <div className='p-3' style={{backgroundColor:"rgb(221, 221, 221)", height:"100%"}}>
          {getTasksByStatus('DONE').map(task => (
            <TaskCard key={task.id} task={task} projectId={projectId} />
          ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default KanbanBoard;
