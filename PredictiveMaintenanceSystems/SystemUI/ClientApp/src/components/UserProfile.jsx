import React from 'react'
import {makeStyles} from '@material-ui/core/styles'
import Card from '@material-ui/core/Card'
import CardActionArea from '@material-ui/core/CardActionArea'
import CardHeader from '@material-ui/core/CardHeader'
import Typography from '@material-ui/core/Typography'
import Avatar from '@material-ui/core/Avatar'
import {red} from '@material-ui/core/colors'
import getData from './JSONHandler'



const UserProfile: React.FC = (data) => {
    const classes = useStyles();
    return (
        <div>
            <Card className={classes.card}>
                <CardActionArea>
                    <CardHeader
                        avatar={
                            <Avatar aria-label="recipe" className={classes.avatar}>
                                J
                            </Avatar>
                        }
                        title={
                            <Typography gutterBottom variant="h5" component="h2">John Smith</Typography>
                        }
                        subheader={
                            <Typography variant="body2" color="textSecondary" component="h3">
                                The Boring Company
                            </Typography>
                        }/>
                </CardActionArea>
            </Card>
        </div>
    )
};

const useStyles = makeStyles({
    card: {
        width: 240,
        marginLeft: 10,
        marginBottom: 10,
        alignContent: 'Center'
    },
    media: {
        height: 140,
    },
    Icon: {
        color: '#003366',
    },
    avatar: {
        backgroundColor: red[500],
    },
});

export default UserProfile