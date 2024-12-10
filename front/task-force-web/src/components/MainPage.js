import React from 'react';
import MainHeader from './MainHeader';
import ProjectList from './ProjectList';

const MainPage = () => {
  return (
    <div style={{ backgroundColor: '#f5f5f5', height: '100vh' }}>
      <MainHeader />
      <ProjectList />
    </div>
  );
};

export default MainPage;
