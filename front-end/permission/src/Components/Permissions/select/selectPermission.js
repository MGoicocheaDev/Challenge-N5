import { useEffect, useState } from "react";
import axios from "axios";

const SelectPermission = ({valuePermissionSelected, getPermissionSelected}) => {
    const [permissionListData, permissionListDataChange] = useState(null);
    const API_BASE_URL = "http://localhost:8098/api/PermissionTypes"

    useEffect(() => {
        axios.get(API_BASE_URL).then((response) => {
            permissionListDataChange(response.data);
          });
    }, [])

    const handleChange = (e) =>{
        console.log("select value: " +e.target.value);
        getPermissionSelected(e.target.value);
    }

    return(
        <select className="form-select" onChange={handleChange}>
             {permissionListData &&
                permissionListData.map(item => (
                    <option value={item.id} selected={item.id == valuePermissionSelected} >
                        {item.descripcion}
                    </option>
                ))
            }
        </select>
    );
}
export default SelectPermission;