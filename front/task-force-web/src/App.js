import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import KanbanBoard from './components/KanbanBoard';
import TaskDetails from './components/TaskDetails';

function App() {
  return (
    <Router>
      <Routes>
      <Route path="/" element={<KanbanBoard />} /> {/* Компонент для корневого пути */}
      <Route path="/tasks/:taskId" element={<TaskDetails />} />
      </Routes>
    </Router>
  );
}

export default App;
