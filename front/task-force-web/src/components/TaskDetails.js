import React, { useEffect, useState } from "react";
import { fetchTaskById } from '../services/api';
import { useParams, Link } from 'react-router-dom';
import '../styles/TaskDetails.css'

function TaskDetails() {
  const defaultTask = {
    title: "Task's title",
    tags: ["loading", "loading", "loading"],
    priority: "----",
    author: "-------",
    performer: "-----",
    date: "------",
    status: "-------",
    description: "-----",
  };

  const { taskId } = useParams();
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
    if (task.status === 'to_do')
      return "in_progress";
    return 'to_do';
  }

  const getSecondButtonName = () => {
    if (task.status === 'done')
      return 'in_progress';
    return 'done';
  }

  return (
    <div className="task-modal">
      <div className="task-modal-content">
        <div className="content-modal-header">
        <h2>{task.title}</h2>
        <div className="navigation-buttons">
          <button className="nav-btn">{getFirstButtonName()}</button>
          <button className="nav-btn">{getSecondButtonName()}</button>
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
            <p><strong>Creator:</strong> {task.author}</p>
            <p><strong>Performer:</strong> {task.performer}</p>
          </div>
          <div className="info-column">
            <p><strong>Date:</strong> {task.date}</p>
            <p><strong>Status:</strong> {task.status}</p>
          </div>
        </div>
  
        <textarea 
          className="description" 
          value={task.description} 
          readOnly
        />
  
        <div className="actions">
          <Link className="link-without" to="/">
            <button className="cancel-btn">
              Cancel
            </button>
          </Link>
          <Link className="link-without" to={`/tasks/${task.id}/edit`}>
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