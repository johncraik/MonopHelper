let baseUrl = '../../../../../../../CardAction/'

function ShowPlayCondition(toggle){
    let condd = document.getElementById('con-dd');
    condd.hidden = toggle.checked !== true;
}

function DeleteActions(){
    Swal.fire({
        title: 'Delete Actions?',
        text: 'This cannot be undone.',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((res) => {
        if(res.isConfirmed) {
            let frm = document.getElementById('delete-form');
            frm.submit();
        }
    })
}

function SetGroup(groupId){
    let group = "New Group"
    if(groupId !== "0"){
        group = "Group " + groupId;
    }
    
    document.getElementById('add-group').innerText = group;
    document.getElementById('group-id').value = groupId;
}

function SetInputLabel(id, opt1, opt2){
    let label =  document.getElementById(id);
    let name= label.innerText;
    if(name === opt1){
        name = opt2;
    }
    else{
        name = opt1;
    }
    label.innerText = name;
}

function SetPayLabels(){
    SetInputLabel('pay-val', 'Pay', 'Receive');
    SetInputLabel('payto', 'Pay To', 'Receive From');
}

function SetPropLabels(){
    SetInputLabel('source', 'Take Property From', 'Return Property To')
}

function SetResetInput(list){
    let valInp = document.getElementById('reset-val');
    valInp.hidden = list.value !== "1";
}

function GetEventAction(list, card, group, action){
    let selected = list.value;
    let model = undefined;
    let url = baseUrl + 'GetEditEventPartial?cardId=' + card + '&groupId=' + group + '&actionId' + action + '&eventType=' + selected;
    
    let eventActions = document.getElementsByClassName('event-actions');
    
    for (let i = 0; i < eventActions.length; i++){
        let div = eventActions[i];
        if(div.id === ('event-' + selected)){
            model = div;
            model.hidden = false;
        }
        else{
            div.hidden = true;
        }
    }
    
    FetchPartial(url, model);
}

function ShowMultiplier(list){
    let val = document.getElementById('multi-val');
    let type = document.getElementById('multi-type');

    if (list.value === "3"){
        val.hidden = false;
        type.hidden = false;
    }
    else{
        val.hidden = true;
        type.hidden = true;
    }
}

function ShowFields(list){
    let turns = document.getElementById('jail-turns');
    let swap = document.getElementById('jail-swap');

    if (list.value === "0"){
        turns.hidden = false;
        swap.hidden = true;
    }
    else if (list.value === "1"){
        swap.hidden = false;
        turns.hidden = true;
    }
    else{
        turns.hidden = true;
        turns.hidden = true;
    }
}