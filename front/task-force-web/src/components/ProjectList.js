import React, { useEffect, useState } from 'react';
import ProjectCard from './ProjectCard.js';
import { getAllProject } from '../services/api';

const ProjectList = () => {
  const [projects, setProjects] = useState([]);
  useEffect(() => {
      // Получаем задачи при загрузке страницы
      const getProjects = async () => {
        const fetchedProjects = await getAllProject();
        setProjects(fetchedProjects);
      };
  
      getProjects();
    }, []);
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
