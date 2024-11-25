function ShowCards(decks){
    let selectedIndex = decks.options.selectedIndex;
    let deck = decks.options[selectedIndex].value;
    let url = "../Game/CardsTablePartial?id=" + deck;
    let model = document.getElementById("CardsTable");
    
    FetchPartial(url, model);
}