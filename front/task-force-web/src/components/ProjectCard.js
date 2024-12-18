import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/ProjectCard.css'

const ProjectCard = ({id, name, tasksCount, usersCount}) => {
  return (
    <Link class='link-project' to={`/${id}`}>
        <div style={{ fontSize: '18px', fontWeight: 'bold' }}>{name}</div>
        <div style={{ display: 'flex', alignItems: 'center', gap: '20px' }}>
          <span style={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
            {tasksCount} 📋
          </span>
          <span style={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
            {usersCount} 👤
          </span>
        </div>
    </Link>
  );
};

export default ProjectCard;
