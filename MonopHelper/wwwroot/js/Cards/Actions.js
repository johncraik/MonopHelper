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