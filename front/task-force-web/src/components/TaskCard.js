import React from 'react';
import { Link } from 'react-router-dom';

const priorityColors = {
  'very high': '#f00',
  'high': '#ff5e5e',
  'medium': '#fac337',
  'low': '#eafa37',
  'lowest': '#83ff08',
};

const TaskCard = ({ task }) => {

  const getAuthor = () => {
    if (task.author == null)
      return (<p/>)
    return (
        <p style={{'margin-bottom': '0px', display: 'flex', 'justify-content': 'center'}}>
        <img src="/images/user.png" width="20" height="20" />
        {task.author}</p>
    )
  }

  const getDate = () => {
    if (task.date == null)
      return (<p/>)
    return (
      <p style={{'margin-bottom': '0px', display: 'flex', 'justify-content': 'center'}}>
        <img src="/images/clock.png" width="20" height="20" />
        {task.date}</p>
    )
  }

  return (
    <Link to={`/tasks/${task.id}`} class="card text-dark mb-3 text-decoration-none" style={{height: '9rem', overflow: 'hidden', boxShadow: '2px 2px 5px grey'}}>
      <div className="card-body d-flex flex-column card-body-hover" style={{'justify-content': 'space-between', 'padding-bottom': '5px'}}>
        <div
        className="priority-indicator"
        style={{ backgroundColor: priorityColors[task.priority] }}
        />
        <h5 class="card-title text-truncate-2">{task.title}</h5>
        <div>
          <div className="task-hashtags" style={{'margin-bottom': '3px'}}>
            {task.hashtags.slice(0, 6).map((tag, index) => (
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
