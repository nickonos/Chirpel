import React from "react";
import {Menu} from "primereact/menu";

class Menu extends React.Component{
    constructor(props) {
        super(props);
        this.items = [
            {
                label: 'Options',
                items: [
                    {
                        label: 'Update',
                        icon: 'pi pi-refresh',
                        command: () => {
                            this.toast.show({ severity: 'success', summary: 'Updated', detail: 'Data Updated', life: 3000 });
                        }
                    },
                    {
                        label: 'Delete',
                        icon: 'pi pi-times',
                        command: () => {
                            this.toast.show({ severity: 'warn', summary: 'Delete', detail: 'Data Deleted', life: 3000 });
                        }
                    }
                ]
            },
            {
                label: 'Navigate',
                items: [
                    {
                        label: 'React Website',
                        icon: 'pi pi-external-link',
                        url: 'https://reactjs.org/'
                    },
                    {
                        label: 'Router',
                        icon: 'pi pi-upload',
                        command:(e) => {
                            window.location.hash = "/fileupload"
                        }
                    }
                ]
            }
        ];
    }
    render() {
        return<Menu></Menu>
    }
}
export default Menu