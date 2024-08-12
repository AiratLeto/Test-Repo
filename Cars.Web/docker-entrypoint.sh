#!/usr/bin/env sh
set -eu
envsubst '${VITE_APP_API_URL}' < /etc/nginx/conf.d/default.conf.template > /etc/nginx/conf.d/default.conf
exec "$@"