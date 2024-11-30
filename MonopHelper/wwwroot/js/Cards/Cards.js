function ShowCards(decks){
    let selectedIndex = decks.options.selectedIndex;
    let deck = decks.options[selectedIndex].value;
    
    document.getElementById("DeckId").value = deck;
    FetchCards(deck);
}

function FetchCards(deckId){
    let url = "../Card/CardsTablePartial?id=" + deckId;
    let model = document.getElementById("CardsTable");
    FetchPartial(url, model);
}

function SetUploadType(id, name){
    document.getElementById("TypeId").value = id;
    document.getElementById("CardTypeName").innerText = name;
}

function DeleteCard(id){
    Swal.fire({
        title: "Delete Card?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: '../Card/RemoveCard?id=' + id,
                success: function (res){
                    Swal.fire("Card Deleted", "", "success").then((r) => {
                        FetchCards(id)
                    });
                }
            })
            
        }
    })
}