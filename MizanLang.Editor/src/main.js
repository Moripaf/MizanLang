import { createMizanEditor } from "./editor.js";


let ThemeIsLight = false;
const root = document.getElementById("editor-root");
function render() {
    const initialCode =
        "اگر [Personnel.Age] بزرگتر از 30 باشد\nباید [Personnel.Salary] > 5000 مقدار است";

    createMizanEditor({ domElement: root, initialValue: initialCode, onChange: console.log, isLightMode: ThemeIsLight});
}
document.getElementById('toggleTheme').addEventListener('click', () => {
    ThemeIsLight = !ThemeIsLight;
    // root.innerHTML = '';
    while (root.firstChild) {
        root.removeChild(root.firstChild);
    }
    if(ThemeIsLight)
        root.classList.add("light")
    else
        root.classList.remove("light")
    render();
})
render();