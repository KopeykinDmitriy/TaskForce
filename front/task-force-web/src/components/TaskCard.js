import React from 'react';
import { Link } from 'react-router-dom';

const TaskCard = ({ task }) => {
  return (
    <div className="card mb-3">
      <div className="card-body">
        <h5 className="card-title">{task.title}</h5>
        <p className="card-text">{task.description}</p>
        <Link to={`/tasks/${task.id}`} className="btn btn-secondary">
          Подробнее
        </Link>
      </div>
    </div>
  );
};

export default TaskCard;
