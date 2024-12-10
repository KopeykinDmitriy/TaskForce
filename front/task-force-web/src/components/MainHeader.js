import React from 'react';
import { Link } from 'react-router-dom';

const MainHeader = () => {
  return (
    <div style={{
      backgroundColor: '#3a7bd5',
      color: 'white',
      padding: '20px',
      textAlign: 'center',
      fontFamily: 'Arial, sans-serif',
    }}>
      <h1>Hello, user_name!</h1>
      <p>Welcome to Smart Collaboration Tools</p>
      <p>
        Choose your project or{' '}
        <Link to="/create-new-project">
        <button
          style={{
            backgroundColor: '#ff6600',
            color: 'white',
            border: 'none',
            padding: '5px 10px',
            cursor: 'pointer',
            borderRadius: '5px',
          }}
        >
          create new
        </button>
        </Link>
        <Link to="/logout">
        <button
        style={{
          position: 'absolute', // Абсолютное позиционирование
          top: '20px',
          right: '20px',
          width: '80px', // Квадратная форма
          height: '40px',
          backgroundColor: '#ffffff',
          border: 'none',
          cursor: 'pointer',
          boxShadow: '0px 4px 6px rgba(0, 0, 0, 0.2)',
          borderRadius: '5px', // Скруглённые углы
        }}
      >
        Выйти
      </button>
      </Link>
      </p>
    </div>
  );
};

export default MainHeader;
