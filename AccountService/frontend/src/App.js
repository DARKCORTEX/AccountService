import React, {useState} from 'react';

function App() {
  const [userNameInputText, setUserNameInputText] = useState("");
  const [userPasswordInputText, setUserPasswordInputText] = useState("");
  const [mensaje, setMensaje] = useState('');

  const userNameHandleChange = (event) => {
    setUserNameInputText(event.target.value);
  };

  const userPasswordHandleChange = (event) => {
    setUserPasswordInputText(event.target.value);
  };

  const handleClear = () => {
    setUserNameInputText("");
    setUserPasswordInputText("");
  };

  /*const Register = () =>
  {

    handleClear();
  }*/

  const Login = () =>
  {
    fetch('http://localhost:5110/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ userName: userNameInputText, userPassword: userPasswordInputText })
  })
    .then(res => {
      if (!res.ok) throw new Error("Credenciales inválidas");
      return res.json();
    })
    .then(data => setMensaje(data.message))
    .catch(err => setMensaje(err.message));
  }

  return (
    <div style={{ padding: '2rem', fontFamily: 'sans-serif' }}>
      <h1>User Name</h1>
      
      <input
        type="text"
        placeholder="User Name"
        value={userNameInputText}
        onChange={userNameHandleChange}
        style={{ padding: '0.5rem', fontSize: '1rem', marginRight: '1rem' }}
      />

      <h1>User Passowrd</h1>
      
      <input
        type="password"
        placeholder="User Password"
        value={userPasswordInputText}
        onChange={userPasswordHandleChange}
        style={{ padding: '0.5rem', fontSize: '1rem', marginRight: '1rem' }}
      />

      <button onClick={Login} style={{ padding: '0.5rem 1rem' }}>
        Login
      </button>

      {/*<p>Texto actual: <strong>{inputText}</strong></p>*/}
      {mensaje && (
        <p style={{ marginTop: '1rem', color: 'green' }}>
          {mensaje}
        </p>
      )}
    </div>
  );
}

export default App;
