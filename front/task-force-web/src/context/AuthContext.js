import React, { createContext, useContext, useState } from 'react';

const AuthContext = createContext();

// Хук для удобного доступа к контексту
export const useAuth = () => {
  return useContext(AuthContext);
};

// Провайдер для авторизации
export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(
    Boolean(localStorage.getItem('authToken')) // Проверяем, есть ли токен при загрузке
  );

  const login = (token) => {
    localStorage.setItem('authToken', token); // Сохраняем токен
    setIsAuthenticated(true);
  };

  const logout = () => {
    localStorage.removeItem('authToken'); // Удаляем токен
    setIsAuthenticated(false);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
