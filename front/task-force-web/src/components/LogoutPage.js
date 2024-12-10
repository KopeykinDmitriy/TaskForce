import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const LogoutPage = () => {
  const navigate = useNavigate();

  const { logout } = useAuth();

  useEffect(() => {
    // Очищаем токен или любую информацию о пользователе
    logout()
    
    // Перенаправляем на страницу входа
    navigate('/login');
  }, [navigate]);

  return (
    <div>
      <h2>Выход...</h2>
    </div>
  );
};

export default LogoutPage;