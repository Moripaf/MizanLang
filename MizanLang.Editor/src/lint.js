import { syntaxTree } from "@codemirror/language";

export function clientLinter(view) {
    const diagnostics = [];

    // Walk the AST looking for invalid or incomplete syntax nodes
    syntaxTree(view.state).cursor().iterate(node => {
        if (node.type.isError) {
            diagnostics.push({
                from: node.from,
                to: node.to,
                severity: "error",
                message: "Syntax error: Unexpected token or missing construct."
            });
        }
    });

    return diagnostics;
}