import React from 'react';
import ProjectCard from './ProjectCard.js';

const projects = [
  { id: 1, name: 'My Best Project', tasks: 48, users: 12 },
  { id: 2, name: 'My Best Project', tasks: 48, users: 12 },
  { id: 3, name: 'My Best Project', tasks: 48, users: 12 },
  { id: 4, name: 'My Best Project', tasks: 48, users: 12 },
  { id: 5, name: 'My Best Project', tasks: 48, users: 12 },
];

const ProjectList = () => {
  return (
    <div style={{
      padding: '20px',
      display: 'flex',
      flexDirection: 'column',
      gap: '10px',
      alignItems: 'center',
    }}>
      {projects.map((project) => (
        <ProjectCard key={project.id} {...project} />
      ))}
    </div>
  );
};

export default ProjectList;
