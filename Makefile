CC=gcc
CFLAGS=-pthread -Wall -Wextra
BIN_DIR=bin
SRC_DIR=src

all: $(BIN_DIR)/producer_consumer $(BIN_DIR)/readers_writers $(BIN_DIR)/dining_philosophers

$(BIN_DIR)/producer_consumer: $(SRC_DIR)/producer_consumer.c | $(BIN_DIR)
	$(CC) $(CFLAGS) $< -o $@

$(BIN_DIR)/readers_writers: $(SRC_DIR)/readers_writers.c | $(BIN_DIR)
	$(CC) $(CFLAGS) $< -o $@

$(BIN_DIR)/dining_philosophers: $(SRC_DIR)/dining_philosophers.c | $(BIN_DIR)
	$(CC) $(CFLAGS) $< -o $@

$(BIN_DIR):
	mkdir -p $(BIN_DIR)

clean:
	rm -rf $(BIN_DIR)
