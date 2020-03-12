async function getData() {
    try {
        let response = await fetch('https://localhost:5001/api/User');
        let responseJson = await response.json();
        console.log(responseJson)
        return responseJson;
    } catch(error) {
        console.error(error);
    }
}

async function postData() {
    try {
        let xhr = new XMLHttpRequest();
        let url = "localhost:3002/api";
        xhr.open("POST", url, true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                let json = JSON.parse(xhr.responseText);
                console.log(json);
            }
        };
        let data = JSON.stringify({"Data": {"TrainSetSource": {"DataSourceID": "123"}}});
        xhr.send(data);
    } catch(error) {
        console.error(error);
    }
}