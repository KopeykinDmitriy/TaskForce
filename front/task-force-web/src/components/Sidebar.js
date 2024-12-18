import React from 'react';
import { Link, useParams } from 'react-router-dom';
import '../styles/Sidebar.css'

const Sidebar = () => {
  const { projectId } = useParams();
  return (
    <div className="sidebar">
      <div class="sidebar-top">
        <div class="sidebar-logo">
          <img src="/images/logo-minimal-bg.jpg" width="100" height="50" />
        </div>
        <button class="sidebar-btn" type="button"><Link class='link-without' to="/projects">Home</Link></button>
        <button class="sidebar-btn" type="button"><Link class='link-without' to={`/${projectId}`}>Kanban</Link></button>
        <button class="sidebar-btn" type="button"><Link class='link-without' to={`/${projectId}/tasks/create`}>New Task</Link></button>
      </div>
      <div class="sidebar-bot">
        <button class="sidebar-btn" type="button"><Link class='link-without' to={`/${projectId}/settings/users`}>Users</Link></button>
        <button class="sidebar-btn" type="button"><Link class='link-without' to="/logout">Logout</Link></button>
      </div>
    </div>
  );
};

export default Sidebar;
