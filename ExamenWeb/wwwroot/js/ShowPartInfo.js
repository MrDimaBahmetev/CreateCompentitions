var radios = document.getElementsByName("tab-control");
var actionOpensInfo = document.getElementById("OpenPartInfo");
actionOpensInfo.onclick = function () {
    if (!radios[1].checked) {
        radios[1].checked = true; // поставить checked на input с блоком информации, если он не установлен
    }
}