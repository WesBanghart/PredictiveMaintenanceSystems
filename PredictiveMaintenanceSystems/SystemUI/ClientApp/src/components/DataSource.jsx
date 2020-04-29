import React, {forwardRef} from 'react';
import MaterialTable, {Column, Icons} from 'material-table';
import AddBox from '@material-ui/icons/AddBox';
import ArrowUpward from '@material-ui/icons/ArrowUpward';
import Check from '@material-ui/icons/Check';
import ChevronLeft from '@material-ui/icons/ChevronLeft';
import ChevronRight from '@material-ui/icons/ChevronRight';
import Clear from '@material-ui/icons/Clear';
import DeleteOutline from '@material-ui/icons/DeleteOutline';
import Edit from '@material-ui/icons/Edit';
import FilterList from '@material-ui/icons/FilterList';
import FirstPage from '@material-ui/icons/FirstPage';
import LastPage from '@material-ui/icons/LastPage';
import Remove from '@material-ui/icons/Remove';
import SaveAlt from '@material-ui/icons/SaveAlt';
import Search from '@material-ui/icons/Search';
import ViewColumn from '@material-ui/icons/ViewColumn';
import Typography from "@material-ui/core/Typography";

const tableIcons: Icons = {
    Add: forwardRef((props, ref) => <AddBox {...props} ref={ref}/>),
    Check: forwardRef((props, ref) => <Check {...props} ref={ref}/>),
    Clear: forwardRef((props, ref) => <Clear {...props} ref={ref}/>),
    Delete: forwardRef((props, ref) => <DeleteOutline {...props} ref={ref}/>),
    DetailPanel: forwardRef((props, ref) => <ChevronRight {...props} ref={ref}/>),
    Edit: forwardRef((props, ref) => <Edit {...props} ref={ref}/>),
    Export: forwardRef((props, ref) => <SaveAlt {...props} ref={ref}/>),
    Filter: forwardRef((props, ref) => <FilterList {...props} ref={ref}/>),
    FirstPage: forwardRef((props, ref) => <FirstPage {...props} ref={ref}/>),
    LastPage: forwardRef((props, ref) => <LastPage {...props} ref={ref}/>),
    NextPage: forwardRef((props, ref) => <ChevronRight {...props} ref={ref}/>),
    PreviousPage: forwardRef((props, ref) => <ChevronLeft {...props} ref={ref}/>),
    ResetSearch: forwardRef((props, ref) => <Clear {...props} ref={ref}/>),
    Search: forwardRef((props, ref) => <Search {...props} ref={ref}/>),
    SortArrow: forwardRef((props, ref) => <ArrowUpward {...props} ref={ref}/>),
    ThirdStateCheck: forwardRef((props, ref) => <Remove {...props} ref={ref}/>),
    ViewColumn: forwardRef((props, ref) => <ViewColumn {...props} ref={ref}/>)
};

interface Row {
    device: string;
    location: string;
    ip: string;
}
// eslint-disable-next-line
interface TableState {
    columns: Array<Column<Row>>;
    data: Row[];
}

export default class Devices extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            columns: [
                {title: 'Data Source', field: 'device'},
                {title: 'ID', field: 'id'},
                {title: 'Connection', field: 'connectionstring'},
                {title: 'Last Updated', field: 'lastupdated'},

            ],
            data: [],
        }
    }

    componentDidMount() {
        let tmpData = [];
        for(let i = 0; i < this.props.dataSourceData.length; ++i) {
            tmpData[i] = {
                device: this.props.dataSourceData[i]["dataSourceName"],
                id: this.props.dataSourceData[i]["dataSourceId"],
                connectionstring: this.props.dataSourceData[i]["connectionString"],
                lastupdated: this.props.dataSourceData[i]["lastUpdated"]
            };
        }
        this.setState({data: tmpData});
    }

    render() {
        return (
            <div>
                <Typography color="textPrimary" variant="h4" component="h2">Data Sources</Typography>
                <br/>
                <MaterialTable
                    title="Data Source List"
                    columns={this.state.columns}
                    data={this.state.data}
                    icons={tableIcons}
                    editable={{
                        onRowAdd: newData =>
                            new Promise(resolve => {
                                setTimeout(() => {
                                    resolve();
                                    this.setState(prevState => {
                                        const data = [...prevState.data];
                                        data.push(newData);
                                        return {...prevState, data};
                                    });
                                }, 600);
                            }),
                        onRowUpdate: (newData, oldData) =>
                            new Promise(resolve => {
                                setTimeout(() => {
                                    resolve();
                                    if (oldData) {
                                        this.setState(prevState => {
                                            const data = [...prevState.data];
                                            data[data.indexOf(oldData)] = newData;
                                            return {...prevState, data};
                                        });
                                    }
                                }, 600);
                            }),
                        onRowDelete: oldData =>
                            new Promise(resolve => {
                                setTimeout(() => {
                                    resolve();
                                    this.setState(prevState => {
                                        const data = [...prevState.data];
                                        data.splice(data.indexOf(oldData), 1);
                                        return {...prevState, data};
                                    });
                                }, 600);
                            }),
                    }}
                />
            </div>
        );
    }
}