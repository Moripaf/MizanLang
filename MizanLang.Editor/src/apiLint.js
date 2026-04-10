let controller = null;

export async function serverLinter(view) {
  // Cancel previous fetch if user continues typing
  if (controller) controller.abort();
  controller = new AbortController();

  const code = view.state.doc.toString();
  if (!code.trim()) return [];

  try {
    const response = await fetch('/api/dsl/validate', {
      method: 'POST',
      body: JSON.stringify({ code }),
      signal: controller.signal,
      headers: { 'Content-Type': 'application/json' }
    });

    if (!response.ok) return [];

    const errors = await response.json();

    // Map backend line/column data to CodeMirror positions
    return errors.map(err => {
      const line = view.state.doc.line(err.line);
      return {
        from: line.from + err.column,
        to: line.from + err.column + 1,
        severity: "error",
        message: err.message
      };
    });
  } catch (e) {
    // Ignore intentionally aborted requests
    if (e.name === "AbortError") return [];
    console.error("API Validation failed:", e);
    return [];
  }
}
