FROM gitpod/workspace-dotnet-vnc

# Install custom tools, runtimes, etc.
# For example "bastet", a command-line tetris clone:
# RUN brew install bastet
#
# More information: https://www.gitpod.io/docs/config-docker/

USER gitpod
RUN sudo bash install-dotnet-preview.sh
