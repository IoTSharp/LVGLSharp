#!/usr/bin/env bash

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
RUBY_VERSION="${RUBY_VERSION:-3.2.10}"
HOST="${HOST:-0.0.0.0}"
PORT="${PORT:-4000}"
DESTINATION="${DESTINATION:-/tmp/lvglsharp-docs-build}"

export PATH="$HOME/.rbenv/bin:$HOME/.rbenv/shims:$PATH"

if ! command -v rbenv >/dev/null 2>&1; then
  echo "rbenv is required. Install it first, then rerun this script." >&2
  exit 1
fi

eval "$(rbenv init - bash)"

if ! rbenv versions --bare | grep -Fxq "$RUBY_VERSION"; then
  cat >&2 <<EOF
Ruby $RUBY_VERSION is not installed in rbenv.
Install it with:
  rbenv install $RUBY_VERSION
EOF
  exit 1
fi

rbenv shell "$RUBY_VERSION"

cd "$ROOT_DIR"

if ! bundle check >/dev/null 2>&1; then
  bundle install
fi

cat <<EOF
Docs preview is starting...
  Source:      $ROOT_DIR/docs
  Destination: $DESTINATION
  Preview URL: http://127.0.0.1:$PORT/
EOF

exec bundle exec jekyll serve \
  --source docs \
  --destination "$DESTINATION" \
  --host "$HOST" \
  --port "$PORT"
