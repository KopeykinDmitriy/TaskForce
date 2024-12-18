import React, { useEffect, useState } from "react";
import { fetchTaskById, updateTask } from '../services/api';
import { useParams, Link } from 'react-router-dom';
import '../styles/TaskDetails.css'

function TaskDetails() {
  const defaultTask = {
    name: "Task's title",
    tags: [],
    priority: 'Medium',
    endDateTime: '',
    startDateTime: '',
    status: 'TO_DO',
    description: ``,
    projectId: 0,
    creatorName: '',
    executorName: ''
  };

  const { projectId, taskId } = useParams();
  const [task, setTask] = useState(defaultTask);
  let isInitialized = false;

  const fetchTask = async (id) => {
    try {
      const data = await fetchTaskById(id);
      setTask(data);
      console.log(data);
    } catch (error) {
      console.error("Ошибка при загрузке задачи:", error);
    }
    isInitialized = true;
  };

  useEffect(() => {
    fetchTask(taskId);
  }, [isInitialized]);

  const getFirstButtonName = () => {
    if (task.status === 'TO_DO')
      return "IN_PROGRESS";
    return 'TO_DO';
  }

  const getSecondButtonName = () => {
    if (task.status === 'DONE')
      return 'IN_PROGRESS';
    return 'DONE';
  }

  const onClickFirstButton = async () => {
    task.status = getFirstButtonName();
    await updateTask(task);
    setTask((prevTask) => ({ ...prevTask, status: task.status }));
  }

  const onClickSecondButton = async () => {
    task.status = getSecondButtonName();
    await updateTask(task);
    setTask((prevTask) => ({ ...prevTask, status: task.status }));
  }

  return (
    <div className="task-modal">
      <div className="task-modal-content">
        <div className="content-modal-header">
        <h2>{task.name}</h2>
        <div className="navigation-buttons">
          <button className="nav-btn" onClick={onClickFirstButton}>{getFirstButtonName()}</button>
          <button className="nav-btn" onClick={onClickSecondButton}>{getSecondButtonName()}</button>
        </div>
        </div>
        <div className="tags">
          {task.tags.map((tag, index) => (
            <span key={index} className="tag">
              {tag}
            </span>
          ))}
        </div>
  
        <p className="priority">
          {task.priority}
        </p>
  
        <div className="info">
          <div className="info-column">
            <p><strong>Creator:</strong> {task.creatorName}</p>
            <p><strong>Performer:</strong> {task.executorName}</p>
          </div>
          <div className="info-column">
            <p><strong>StartDate:</strong> {task.startDateTime.slice(0, 10)}</p>
            <p><strong>Status:</strong> {task.status}</p>
          </div>
          <div className="info-column">
            <p><strong>EndDate:</strong> {task.endDateTime.slice(0, 10)}</p>
          </div>
        </div>
  
        <textarea 
          className="description" 
          value={task.description} 
          readOnly
        />
  
        <div className="actions">
          <Link className="link-without" to={`/${projectId}`}>
            <button className="cancel-btn">
              Cancel
            </button>
          </Link>
          <Link className="link-without" to={`/${projectId}/tasks/${task.id}/edit`}>
            <button className="edit-btn">
              Edit
            </button>
          </Link>
        </div>
      </div>
    </div>
  );
  
}

export default TaskDetails;