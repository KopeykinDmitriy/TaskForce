import React, { createContext, useContext, useState, useEffect } from 'react';
import axios from 'axios';

const AuthContext = createContext();

const decodeToken = (token) => {
  var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
};

export const useAuth = () => {
  return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
  const [authenticated, setAuthenticated] = useState(false);
  const [token, setToken] = useState(null);
  const [loading, setLoading] = useState(true); // Добавляем флаг загрузки

  useEffect(() => {
    const storedToken = localStorage.getItem('authToken');
    if (storedToken) {
      // Проверяем срок действия токена
      const decodedToken = decodeToken(storedToken);
      console.log(decodedToken)
      if (decodedToken && decodedToken.exp * 1000 > Date.now()) {
        setToken(storedToken);
        setAuthenticated(true);
      } else {
        // Если токен истек, удаляем его
        localStorage.removeItem('authToken');
        setAuthenticated(false);
      }
    }
    setLoading(false);
  }, []);

  const login = async (username, password) => {
    try {
      const response = await axios.post('http://localhost/auth/realms/myRealm/protocol/openid-connect/token', new URLSearchParams({
        client_id: 'SCT-client',
        client_secret: 'IJHbOv8aNBhZvWHlumKhIJZKjn47PsCU', // Укажите client_secret, если используется
        username,
        password,
        grant_type: 'password',
      }));

      const { access_token } = response.data;
      localStorage.setItem('authToken', access_token);
      setToken(access_token);
      setAuthenticated(true);
    } catch (error) {
      console.error("Error logging in", error);
      alert("Неверные учетные данные");
    }
  };

  const logout = () => {
    localStorage.removeItem('authToken');
    setToken(null);
    setAuthenticated(false);
  };

  return (
    <AuthContext.Provider value={{ authenticated, token, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
};
