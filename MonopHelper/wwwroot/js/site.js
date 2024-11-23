function ShowAddPlayer(){
    let addPlayers = document.getElementById("AddPlayers");
    addPlayers.hidden = false;
}

function AddPlayer(){
    let player = EnsurePlayers();
    
    let playerDisplay = document.getElementById("GamePlayers");
    let url = "Game/NewGamePlayersPartial?players=" + encodeURIComponent(player);

    fetch(url).then(data => {return data.text()}).then(body => {
        playerDisplay.innerHTML = body;
        var scripts = playerDisplay.querySelectorAll('script');
        for (let i = 0; i < scripts.length; i++) {
            if (scripts[i].type !== "text/x-template") {
                eval(scripts[i].innerHTML);
            }
        }

        let addPlayers = document.getElementById("AddPlayers");
        addPlayers.hidden = true;
    });
}

function EnsurePlayers(){
    let inp = document.getElementById("PlayerName");
    let player = inp.value + "/#/";
    inp.value = "";
    let currPlayers = document.getElementsByClassName("player");

    for (let i = 0; i < currPlayers.length; i++){
        player += (currPlayers[i].innerText + "/#/")
    }

    document.getElementById("SetPlayers").value = player;
    return player;
}

function LoadGame(id){
    location.assign("/InGame/" + id)
}


function LeaveJail(id, name){
    Swal.fire({
        title: "Are you sure you want " + name + " to leave jail?",
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "No"
    }).then((result) => {
        if(result.isConfirmed){
            Swal.fire(name + " has left jail!", "", "success").then((r) => {
                $.ajax({
                    type: 'POST',
                    url: '../Game/LeaveJail?id=' + id,
                    success: function (game){
                        location.assign("/InGame/" + game);
                    }
                })
            });
        }
    })
}
