import { createMizanEditor } from "./editor.js";

const root = document.getElementById("editor-root");
const initialCode =
  "اگر [Personnel.Age] بزرگتر از 30 باشد\nباید [Personnel.Salary] > 5000 مقدار است";

createMizanEditor({ domElement: root, initialValue: initialCode, onChange: console.log });
