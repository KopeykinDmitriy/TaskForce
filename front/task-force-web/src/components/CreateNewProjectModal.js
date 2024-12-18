import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { addProject } from '../services/api';

const CreateNewProjectModal = () => {
    const [projectName, setProjectName] = useState('');

    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        const project = {name: projectName, description:''};
        await addProject(project);
        navigate('/')
      };

    return (
    <div class="base-container">
        <div class="base-box">
            <h1 class="base-title">Create new project</h1>
            <form class="base-form" onSubmit={handleSubmit}>
                <div class="input-group">
                    <label>
                        <i class="fa fa-user"></i>
                    </label>
                    <input type="text" id="projectName" name="projectName" placeholder="Project Name" required value={projectName} onChange={(e) => setProjectName(e.target.value)} />
                </div>
                <button type='submit' class="btn-submit">Create</button>
            </form>
        </div>
    </div>
    );
};

export default CreateNewProjectModal;