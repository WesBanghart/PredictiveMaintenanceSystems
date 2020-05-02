import React from 'react';
import {makeStyles} from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardHeader from '@material-ui/core/CardHeader';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import IconButton from '@material-ui/core/IconButton';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import {red} from '@material-ui/core/colors';
import {Chart} from "react-google-charts";
import Table from "./Table";

//Create the cards for the desktop view
//This is the first graph and initial card
const AppMenu: React.FC = () => {
    const classes = useStyles();
    return (
        <div>
            <Typography color="textPrimary" variant="h4" component="h2">The Boring Company</Typography>
            <br/>
            <div style={{display: 'flex', justifyContent: 'center'}}>
                <Card className={classes.card}>
                    <CardHeader
                        action={
                            <IconButton aria-label="settings">
                                <FormControl variant="outlined" className={classes.formControl}>
                                    <InputLabel id="demo-simple-select-outlined-label">
                                        Device 1
                                    </InputLabel>
                                    <Select
                                        labelId="demo-simple-select-outlined-label"
                                        id="demo-simple-select-outlined"
                                    >
                                        <MenuItem value="">
                                            <em>None</em>
                                        </MenuItem>
                                        <MenuItem value={10}>Device 1</MenuItem>
                                        <MenuItem value={20}>Device 2</MenuItem>
                                        <MenuItem value={30}>Device 3</MenuItem>
                                    </Select>
                                </FormControl>
                            </IconButton>
                        }
                        title="Device 1"
                        subheader="Next Scheduled Maintenance date: 12/25/2020"
                    />
                    <CardActionArea>
                        <CardContent>
                            <Chart
                                width={'900'}
                                height={'230px'}
                                chartType="LineChart"
                                loader={<div>Loading Chart</div>}
                                data={[
                                    [
                                        {type: 'date', label: 'Day'},
                                        'Number of Failures',
                                    ],
                                    [new Date(2020, 0), 7],
                                    [new Date(2020, 1), 4],
                                    [new Date(2020, 2), 1],
                                    [new Date(2020, 3), 2],
                                    [new Date(2020, 4), 1],
                                    [new Date(2020, 5), 1],
                                    [new Date(2020, 6), 2],
                                    [new Date(2020, 7), 5],
                                    [new Date(2020, 8), 8],
                                    [new Date(2020, 9), 10],
                                    [new Date(2020, 10), 9],
                                    [new Date(2020, 11), 15],
                                ]}
                                options={{
                                    hAxis: {
                                        title: 'Date',

                                    },
                                    vAxis: {
                                        title: 'Number of Device Errors and Warnings',
                                    },
                                }}
                                rootProps={{'data-testid': '1'}}
                            />
                        </CardContent>
                    </CardActionArea>
                </Card>
            </div>
            <br/>
            <div style={{display: 'flex', justifyContent: 'center'}}>
                <Card className={classes.card}>
                    <CardHeader

                        title="Upcoming Device Maintenance"
                    />
                    <CardActionArea>
                        <CardContent>
                            <Table/>
                        </CardContent>
                    </CardActionArea>
                </Card>
            </div>
        </div>
    )
};

//Define the styles to use
const useStyles = makeStyles({
    card: {
        width: '90%',
        marginTop: 5
    },
    media: {
        height: 130,
    },
    Icon: {
        color: '#003366',
    },
    avatar: {
        backgroundColor: red[500],
    },
    formControl: {
        margin: 1,
        minWidth: 120,
    }
});

export default AppMenu;