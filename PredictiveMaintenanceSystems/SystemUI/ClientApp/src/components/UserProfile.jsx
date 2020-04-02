import React from 'react';
import {withStyles} from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardHeader from '@material-ui/core/CardHeader';
import Typography from '@material-ui/core/Typography';
import Avatar from '@material-ui/core/Avatar';
import {red} from '@material-ui/core/colors';

const styles = {
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
};

class UserProfile extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loadedData: false,
            firstName: "",
            lastName: "",
        };
    }

    render() {
        const {classes} = this.props;
        if(this.props.userData && !this.state.loadedData) {
            try {
                this.setState({
                    firstName: this.props.userData["firstName"],
                    lastName: this.props.userData["lastName"],
                });
            } catch (error) {
                console.log(error);
            }
        }

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
                                <Typography gutterBottom variant="h5" component="h2">{this.state.firstName} {this.state.lastName}</Typography>
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
    }
};

export default withStyles(styles)(UserProfile);