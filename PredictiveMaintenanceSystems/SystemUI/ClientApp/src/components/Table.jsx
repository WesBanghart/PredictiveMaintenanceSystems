import React from 'react';
import {makeStyles} from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';

const useStyles = makeStyles({
    root: {
        width: '100%',
        overflowX: 'auto',
    },
    table: {
        minWidth: 650,
        height: 180
    },
});

function createData(name: string, value1: string, status: string, lastMaintained: string) {
    return {name, value1, status, lastMaintained};
}

const rows = [
    createData('Device 1', '21', 'Connected', 'March 2020'),
    createData('Device 2', '2', 'Connected', 'April 2021'),
    createData('Device 3', '14', 'Disconnected', 'October 2020'),
];

export default function SimpleTable() {
    const classes = useStyles();

    return (
        <Paper className={classes.root}>
            <Table className={classes.table} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        <TableCell>Device Name</TableCell>
                        <TableCell align="right">Average Failures Per Month</TableCell>
                        <TableCell align="right">Status&nbsp;</TableCell>
                        <TableCell align="right">Next Predicted Date&nbsp;</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {rows.map(row => (
                        <TableRow key={row.name}>
                            <TableCell component="th" scope="row">
                                {row.name}
                            </TableCell>
                            <TableCell align="right">{row.value1}</TableCell>
                            <TableCell align="right">{row.status}</TableCell>
                            <TableCell align="right">{row.lastMaintained}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </Paper>
    );
}