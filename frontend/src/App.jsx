import { useState, useEffect } from 'react';

function App() {
  const [libros, setLibros] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState(true);
  const [theme, setTheme] = useState('light');
  const [formData, setFormData] = useState({
    titulo: '',
    imagen: ''
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [filtroEstado, setFiltroEstado] = useState('todos');

  useEffect(() => {
    const savedTheme = localStorage.getItem('theme') || 'light';
    setTheme(savedTheme);
    document.documentElement.setAttribute('data-theme', savedTheme);
    fetchLibros();
  }, []);

  const toggleTheme = () => {
    const newTheme = theme === 'light' ? 'dark' : 'light';
    setTheme(newTheme);
    localStorage.setItem('theme', newTheme);
    document.documentElement.setAttribute('data-theme', newTheme);
  };

  const fetchLibros = async () => {
    try {
      const response = await fetch('/api/Libros');
      if (!response.ok) throw new Error('Error al cargar');
      const data = await response.json();
      setLibros(data);
      setLoading(false);
    } catch (error) {
      console.error('Error:', error);
      setError('Error al conectar con el servidor');
      setLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    if (!formData.titulo.trim()) {
      setError('Por favor ingresa un titulo');
      return;
    }

    try {
      console.log('Enviando datos:', JSON.stringify(formData));
      
      const response = await fetch('/api/Libros', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });

      console.log('Respuesta:', response.status, response.statusText);

      if (response.ok) {
        setSuccess('Libro agregado exitosamente');
        setFormData({ titulo: '', imagen: '' });
        fetchLibros();
        setTimeout(() => setSuccess(''), 3000);
      } else {
        const errorText = await response.text();
        console.log('Error:', errorText);
        setError('Error al guardar');
      }
    } catch (error) {
      console.error('Error:', error);
      setError('Error de conexion con el servidor');
    }
  };

  const handleDelete = async (id) => {
    if (!confirm('Estas seguro de eliminar este libro?')) return;

    try {
      const response = await fetch(`/api/Libros/${id}`, {
        method: 'DELETE'
      });

      if (response.ok) {
        fetchLibros();
        setSuccess('Libro eliminado');
        setTimeout(() => setSuccess(''), 3000);
      }
    } catch (error) {
      console.error('Error:', error);
      setError('Error al eliminar');
    }
  };

  const handleTogglePrestado = async (id) => {
    try {
      const response = await fetch(`/api/Libros/${id}/prestar`, {
        method: 'PUT'
      });

      if (response.ok) {
        fetchLibros();
      }
    } catch (error) {
      console.error('Error:', error);
      setError('Error al cambiar estado');
    }
  };

  const handleToggleLeido = async (id) => {
    try {
      const response = await fetch(`/api/Libros/${id}/leido`, {
        method: 'PUT'
      });

      if (response.ok) {
        fetchLibros();
      }
    } catch (error) {
      console.error('Error:', error);
      setError('Error al cambiar estado');
    }
  };

  const librosFiltrados = libros.filter(libro => {
    const matchSearch = libro.titulo?.toLowerCase().includes(searchTerm.toLowerCase());
    const matchFiltro = 
      filtroEstado === 'todos' ||
      (filtroEstado === 'prestados' && libro.prestado) ||
      (filtroEstado === 'disponibles' && !libro.prestado) ||
      (filtroEstado === 'leidos' && libro.leido);
    return matchSearch && matchFiltro;
  });

  return (
    <>
      <button className="theme-toggle" onClick={toggleTheme}>
        {theme === 'light' ? '🌙' : '☀️'}
      </button>

      <div className="header">
        <h1>Gestion de Libros</h1>
        <p className="header-subtitle">Administra tu biblioteca personal</p>
      </div>

      {(error || success) && (
        <div className={`message ${error ? 'error' : 'success'}`}>
          {error || success}
        </div>
      )}

      <div className="form-container">
        <h2>Agregar Nuevo Libro</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <div className="form-group">
              <label>Titulo del Libro</label>
              <input
                type="text"
                name="titulo"
                value={formData.titulo}
                onChange={handleInputChange}
                placeholder="Ej: El Quijote de la Mancha"
                required
              />
            </div>
            <div className="form-group">
              <label>URL de Imagen</label>
              <input
                type="text"
                name="imagen"
                value={formData.imagen}
                onChange={handleInputChange}
                placeholder="https://ejemplo.com/imagen.jpg"
              />
            </div>
            <button type="submit" className="btn-primary">Agregar</button>
          </div>
        </form>
      </div>

      <div className="search-container">
        <input
          type="text"
          placeholder="Buscar libro por titulo..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>

      <div className="filter-container">
        <button
          className={`filter-btn ${filtroEstado === 'todos' ? 'active' : ''}`}
          onClick={() => setFiltroEstado('todos')}
        >
          Todos
        </button>
        <button
          className={`filter-btn prestado ${filtroEstado === 'prestados' ? 'active' : ''}`}
          onClick={() => setFiltroEstado('prestados')}
        >
          Prestados
        </button>
        <button
          className={`filter-btn disponible ${filtroEstado === 'disponibles' ? 'active' : ''}`}
          onClick={() => setFiltroEstado('disponibles')}
        >
          Disponibles
        </button>
        <button
          className={`filter-btn leido ${filtroEstado === 'leidos' ? 'active' : ''}`}
          onClick={() => setFiltroEstado('leidos')}
        >
          Leídos
        </button>
      </div>

      {loading ? (
        <div className="loading">Cargando libros...</div>
      ) : librosFiltrados.length === 0 ? (
        <div className="no-libros">
          {searchTerm ? 'No se encontraron libros' : 'No hay libros registrados'}
        </div>
      ) : (
        <div className="libros-grid">
          {librosFiltrados.map((libro) => (
            <div 
              key={libro.id} 
              className={`libro-card ${libro.prestado ? 'prestado' : ''}`}
            >
              {libro.imagen ? (
                <div className="libro-imagen-wrapper">
                  <img
                    src={libro.imagen}
                    alt={libro.titulo}
                    className="libro-imagen"
                    onError={(e) => {
                      e.target.style.display = 'none';
                      e.target.parentElement.classList.add('imagen-error');
                      e.target.parentElement.innerHTML = '<span style="font-size:3rem">📖</span>';
                    }}
                  />
                </div>
              ) : (
                <div className="libro-placeholder">
                  📖
                </div>
              )}
              <div className="card-content">
                <h3>
                  {libro.titulo}
                  {libro.prestado && <span className="prestado-badge">&#10003;</span>}
                  {libro.leido && <span className="leido-badge">&#128214;</span>}
                </h3>
                <div className="card-buttons">
                  <button
                    className={`btn-prestar ${!libro.prestado ? 'devuelto' : 'prestado-btn'}`}
                    onClick={() => handleTogglePrestado(libro.id)}
                  >
                    {libro.prestado ? 'Devolver' : 'Prestar'}
                  </button>
                  <button
                    className={`btn-leido ${libro.leido ? 'leido-yes' : ''}`}
                    onClick={() => handleToggleLeido(libro.id)}
                  >
                    {libro.leido ? 'No leído' : 'Leído'}
                  </button>
                  <button
                    className="btn-delete"
                    onClick={() => handleDelete(libro.id)}
                  >
                    Eliminar
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </>
  );
}

export default App;
