import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import './App.css'
import MainLayout from './pages/MainLayout'
import Login from './pages/Login'
import Register from './pages/Register'
import HomePage from './pages/HomePage'
import Redirect from './pages/Redirect'
import InfoPage from './pages/InfoPage'

function App() {

  const router = createBrowserRouter([{
    path: "",
    element: <MainLayout/>,
    children: [
      {
        index: true,
        element: <HomePage/>
      },
      {
        path: "login",
        element: <Login/>
      },
      {
        path: "register",
        element: <Register/>
      },
      {
        path: ":shortUrl",
        element: <Redirect/>
      },
      {
        path: "view/:id",
        element: <InfoPage/>
      }
    ]
  }])

  return (
    <>
      <RouterProvider router={router}/>
    </>
  )
}

export default App
