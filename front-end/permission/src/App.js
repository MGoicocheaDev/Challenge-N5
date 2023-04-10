import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import ListPermission from '../src/Components/Permissions/ListPermission'
import RequestPermission from './Components/Permissions/RequestPermission';
import EditPermission from './Components/Permissions/EditPermission';
import ViewPermission from './Components/Permissions/ViewPermission';

function App() {
  return (
    <div className="App">
      <h1>Permission App</h1>
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<ListPermission/>}></Route>
          <Route path='/permission/request' element={<RequestPermission/>}></Route>
          <Route path='/permission/edit/:permissionid' element={<EditPermission/>}></Route>
          <Route path='/permission/detail/:permissionid' element={<ViewPermission/>}></Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
