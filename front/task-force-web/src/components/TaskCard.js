import React from 'react';
import { Link, useParams } from 'react-router-dom';
import '../styles/TaskCard.css'

const priorityColors = {
  'VeryHigh': '#f00',
  'High': '#ff5e5e',
  'Medium': '#fac337',
  'Low': '#eafa37',
  'Lowest': '#83ff08',
};

const TaskCard = ({ task, projectId }) => {
  const getAuthor = () => {
    if (task.executorName == "")
      return (<p/>)
    return (
        <p style={{'margin-bottom': '0px', display: 'flex', 'justify-content': 'center'}}>
        <img src="/images/user.png" width="20" height="20" />
        {task.executorName}</p>
    )
  }

  const getDate = () => {
    if (task.endDateTime == null)
      return (<p/>)
    return (
      <p style={{'margin-bottom': '0px', display: 'flex', 'justify-content': 'center'}}>
        <img src="/images/clock.png" width="20" height="20" />
        {task.endDateTime.slice(0,10)}</p>
    )
  }
console.log(projectId);
  return (
    <Link to={`/${projectId}/tasks/${task.id}`} class="card text-dark mb-3 text-decoration-none" style={{height: '9rem', overflow: 'hidden', boxShadow: '2px 2px 5px grey'}}>
      <div className="card-body d-flex flex-column card-body-hover" style={{'justify-content': 'space-between', 'padding-bottom': '5px'}}>
        <div
        className="priority-indicator"
        style={{ backgroundColor: priorityColors[task.priority] }}
        />
        <h5 class="card-title text-truncate-2">{task.name}</h5>
        <div>
          <div class="task-hashtags" style={{'margin-bottom': '3px'}}>
            {task.tags.slice(0, 6).map((tag, index) => (
              <span key={index} className="badge bg-secondary mx-1 my-1 rounded-pill">
                {tag}
              </span>
            ))}
          </div>
          <div style={{display: 'flex', 'justify-content': 'space-between', 'color': 'gray', 'font-size': '13px', 'text-align': 'center'}}>
            {getAuthor()}
            {getDate()}
          </div>
        </div>
      </div>
    </Link>
  );
};

export default TaskCard;
