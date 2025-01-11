import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import KanbanBoard from './components/KanbanBoard';
import TaskDetails from './components/TaskDetails';
import PrivateRoute from './components/PrivateRoute';
import LoginPage from './components/LoginPage';
import { AuthProvider } from './context/AuthContext';
import SidebarContext from './context/SidebarContext';
import LogoutPage from './components/LogoutPage';
import MainPage from './components/MainPage';
import CreateNewProjectModal from './components/CreateNewProjectModal';
import EditCreateTaskForm from './components/EditCreateTaskForm';
import UserManagementPage from './components/UserManagementPage';
import TrainModelPage from './components/TrainModelPage';


const App = () => {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/logout" element={<LogoutPage />} />
          <Route element={<SidebarContext />}>
            <Route path="/:projectId" element={
              <PrivateRoute><KanbanBoard /></PrivateRoute>
            } />
            <Route path="/:projectId/tasks/:taskId" element={
              <PrivateRoute><TaskDetails /></PrivateRoute>
            } />
            <Route path="/:projectId/tasks/:taskId/edit" element={
              <PrivateRoute><EditCreateTaskForm /></PrivateRoute>
            } />
            <Route path="/:projectId/tasks/create" element={
              <PrivateRoute><EditCreateTaskForm /></PrivateRoute>
            } />
            <Route path="/:projectId/settings/users" element={
              <PrivateRoute><UserManagementPage /></PrivateRoute>
            } />
            <Route path="/:projectId/train" element={
              <PrivateRoute><TrainModelPage /></PrivateRoute>
            } />
          </Route>
          <Route path="/projects" element={
            <PrivateRoute><MainPage /></PrivateRoute>
            }/>
          <Route path="/create-new-project" element={
            <PrivateRoute><CreateNewProjectModal /></PrivateRoute>
            }/>
          <Route path="*" element={<Navigate to="/projects" />} />
        </Routes>
      </Router>
    </AuthProvider>  
  );
};

export default App;
