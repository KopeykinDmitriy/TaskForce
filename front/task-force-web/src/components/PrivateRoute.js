import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const PrivateRoute = ({ children }) => {
  const { authenticated, loading } = useAuth();
  
  if (loading) {
    return <div>Loading...</div>;
  }

  return authenticated ? children : <Navigate to="/login" />;
};

export default PrivateRoute;
