import axios from 'axios';

const API_URL = 'http://localhost'; // URL бэкенда

// Функция для получения axios с авторизацией
const getAxiosInstance = () => {
  const token = localStorage.getItem('authToken');
  return axios.create({
    baseURL: API_URL,
    headers: {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
  });
};

export const fetchTasks = async (projectId) => {
  const axiosInstance = getAxiosInstance(); // Создаём axios инстанс с токеном
  const response = await axiosInstance.get(`/task-manager/Tasks/GetAllTasksByProject?projectId=${projectId}`);
  return response.data;
};

export const createTask = async (taskData) => {
  const axiosInstance = getAxiosInstance();
  const response = await axiosInstance.post('/task-manager/Tasks/AddTask', taskData);
  return response.data;
};

export const updateTask = async (taskData) => {
  const axiosInstance = getAxiosInstance();
  const response = await axiosInstance.post('/task-manager/Tasks/UpdateTask', taskData);
  return response.data;
};

export const fetchTaskById = async (id) => {
  const axiosInstance = getAxiosInstance();
  const response = await axiosInstance.get(`/task-manager/Tasks/GetTask/${id}`);
  return response.data;
};

export const getAllTags = async () => {
  const axiosInstance = getAxiosInstance();
  const response = await axiosInstance.get('/task-manager/Tasks/GetAllTags');
  return response.data;
};

export const getProject = async (projectId) => {
  const axiosInstance = getAxiosInstance();
  const response = await axiosInstance.get(`/task-manager/api/Projects/GetProject/${projectId}`);
  return response.data;
};

export const addProject = async (project) => {
  const axiosInstance = getAxiosInstance();
  console.log(project)
  const response = await axiosInstance.post('/task-manager/api/Projects/AddProject', project);
  return response.data;
};

export const getAllProject = async () => {
  const axiosInstance = getAxiosInstance();
  const response = await axiosInstance.get('/task-manager/api/Projects/GetAllProjects');
  return response.data;
};

export const addUser = async (user) => {
  const axiosInstance = getAxiosInstance();
  const response = await axiosInstance.post(`/users/api/Account/Register?login=${user.login}&email=${user.login}@gmail.com&password=${user.password}&role=${user.role}&projectId=${user.projectId}`, user.tags);
  return response.data;
};

export const getUsers = async () => {
  const axiosInstance = getAxiosInstance();
  const response = await axiosInstance.get('/users/api/Data/users');
  return response.data;
};

export const getUsersByProject = async (projectId) => {
  const axiosInstance = getAxiosInstance();
  const response = await axiosInstance.get(`/users/api/Data/users-by-project?projectId=${projectId}`);
  return response.data;
};

export const getUserInfo = async () => {
  const axiosInstance = getAxiosInstance();
  const response = await axiosInstance.get('/users/api/Data/UserInfo');
  return response.data;
}
