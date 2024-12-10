import React from 'react';

const ProjectCard = ({ name, tasks, users }) => {
  return (
    <div style={{
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center',
      backgroundColor: '#d3d3d3',
      padding: '10px 20px',
      borderRadius: '10px',
      width: '80%',
      fontFamily: 'Arial, sans-serif',
    }}>
      <div style={{ fontSize: '18px', fontWeight: 'bold' }}>{name}</div>
      <div style={{ display: 'flex', alignItems: 'center', gap: '20px' }}>
        <span style={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
          {tasks} 📋
        </span>
        <span style={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
          {users} 👤
        </span>
      </div>
    </div>
  );
};

export default ProjectCard;
