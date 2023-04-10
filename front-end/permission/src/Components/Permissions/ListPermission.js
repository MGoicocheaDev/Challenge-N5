import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";

const ListPermission = () => {
    const [permissionData, permissionDataChange] = useState(null);
    const navigate = useNavigate();
    const API_BASE_URL = "http://api-backend/api/Permisions"

    const update = (id) => {
        navigate("/permission/edit/" + id);
    }
    const detail = (id) => {
        navigate("/permission/detail/" + id);
    }
    useEffect(() => {
        axios.get(API_BASE_URL).then((response) => {
            permissionDataChange(response.data);
          });
    }, [])

    return(
        <div className="container">
            <div className="card">
                <div className="card-title">
                    <h2>Permission Listing</h2>
                </div>
                <div className="card-body">
                    <div className="divbtn">
                        <Link to="permission/request" className="btn btn-success">Request Permission</Link>
                    </div>
                    <table className="table table-bordered">
                        <thead className="bg-dark text-white">
                            <tr>
                                <th>First Name</th>
                                <th>Last Name</th>
                                <th>Permission</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {permissionData &&
                                permissionData.map(item => (
                                    <tr key={item.id}>
                                        <td>{item.nombreEmpleado}</td>
                                        <td>{item.apellidoEmpleado}</td>
                                        <td>{item.permissionTypeName}</td>
                                        <td><a onClick={() => { update(item.id) }} className="btn btn-success">Edit</a>
                                            <a onClick={() => { detail(item.id) }} className="btn btn-primary">Details</a>
                                        </td>
                                    </tr>
                                ))
                            }
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}

export default ListPermission;