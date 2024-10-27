import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import KanbanBoard from './components/KanbanBoard';
import TaskDetails from './components/TaskDetails';
import CreateTaskModal from './components/CreateTaskModal';
import Header from './components/Header';

const App = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleCreateTask = () => {
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
  };

  return (
    <Router>
      <Header onCreateTask={handleCreateTask} />
      <div className="container mt-3">
        <Routes>
          <Route path="/" element={<KanbanBoard />} />
          <Route path="/tasks/:taskId" element={<TaskDetails />} />
        </Routes>
      </div>
      <CreateTaskModal showModal={isModalOpen} handleClose={handleCloseModal} />
    </Router>
  );
};

export default App;
