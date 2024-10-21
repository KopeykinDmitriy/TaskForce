import axios from 'axios';

const API_URL = 'http://localhost:3001'; // URL бэкенда

export const fetchTasks = async () => {
  const response = await axios.get(`${API_URL}/tasks`);
  return response.data;
};

export const createTask = async (taskData) => {
  const response = await axios.post(`${API_URL}/tasks`, taskData);
  return response.data;
};

export const updateTask = async (taskId, taskData) => {
  const response = await axios.put(`${API_URL}/tasks/${taskId}`, taskData);
  return response.data;
};

export const deleteTask = async (taskId) => {
  const response = await axios.delete(`${API_URL}/tasks/${taskId}`);
  return response.data;
};

export const fetchTaskById = async (id) => {
    const response = await fetch(`${API_URL}/tasks/${id}`);
    const task = await response.json();
    return task;
  };
  