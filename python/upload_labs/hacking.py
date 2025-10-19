#!/usr/bin/env python3

import math
from typing import Iterator, List, Tuple

class Thing:
    def __init__(self, name: str, max: int = 100) -> None:
        self.name = name
        self.count = 0
        self.max = max

    def copy_into(self, target: 'Thing') -> None:
        target.count = self.count

    @property
    def is_valid(self) -> bool:
        return 0 <= self.count <= self.max

class Hacks:
    def __init__(self) -> None:
        self.launch_payload = Thing('Launch Payload', 1)
        self.build_infection = Thing('Build Infection', 1)
        self.execute_injection = Thing('Execute Injection', 1)

    @classmethod
    def copy(cls, source: 'Hacks') -> 'Hacks':
        target = Hacks()
        source.copy_into(target)
        return target

    @property
    def all(self) -> Iterator[Thing]:
        yield self.launch_payload
        yield self.build_infection
        yield self.execute_injection

    def copy_into(self, target: 'Hacks') -> None:
        source_attributes = self.all
        target_attributes = target.all
        try:
            next(source_attributes).copy_into(next(target_attributes))
        except StopIteration:
            pass

    @property
    def count(self) -> int:
        return sum(map(lambda x: x.count, self.all))

    @property
    def is_valid(self) -> bool:
        return all([
            *map(lambda x: x.is_valid, self.all),
            self.launch_payload.count + self.build_infection.count > 0
        ])

class Skills:
    def __init__(self) -> None:
        self.infect_payload = Thing('Infect Payload', 1)
        self.critical_payload = Thing('Critical Payload', 5)
        self.ghost_attack = Thing('Ghost Attack', 1)
        self.max = 5

    @classmethod
    def copy(cls, source: 'Skills') -> 'Skills':
        target = Skills()
        source.copy_into(target)
        return target

    @property
    def all(self) -> Iterator[Thing]:
        yield self.infect_payload
        yield self.critical_payload
        yield self.ghost_attack

    def copy_into(self, target: 'Skills') -> None:
        source_attributes = self.all
        target_attributes = target.all
        try:
            next(source_attributes).copy_into(next(target_attributes))
        except StopIteration:
            pass

    @property
    def count(self) -> int:
        return sum(map(lambda x: x.count, self.all))

    @property
    def is_valid(self) -> bool:
        return all([
            *map(lambda x: x.is_valid, self.all),
            self.count <= self.max
        ])

class Upgrade(Thing):
    def __init__(self, name: str, cost: int, max: int = 100) -> None:
        super().__init__(name, max)
        self.cost = cost

    def linear(self, base: float, gradient: float) -> float:
        return base + gradient * self.count

class Upgrades:
    def __init__(self) -> None:
        self.payload_damage = Upgrade('Payload Damage', 1)
        self.breach_speed = Upgrade('Breach Speed', 1)
        self.critical_chance = Upgrade('Critical Chance', 1, 10)
        self.critical_multiplier = Upgrade('Critical Multiplier', 2, 5)
        self.infection_damage = Upgrade('Infection Damage', 1)
        self.infection_specialist = Upgrade('Infection Specialist', 4, 1)
        self.sneak_attack = Upgrade('Sneak Attack', 4, 5)

    @classmethod
    def copy(cls, source: 'Upgrades') -> 'Upgrades':
        target = Upgrades()
        source.copy_into(target)
        return target

    @property
    def all(self) -> Iterator[Upgrade]:
        yield self.payload_damage
        yield self.breach_speed
        yield self.critical_chance
        yield self.critical_multiplier
        yield self.infection_damage
        yield self.infection_specialist
        yield self.sneak_attack

    def copy_into(self, target: 'Upgrades') -> None:
        source_attributes = self.all
        target_attributes = target.all
        try:
            next(source_attributes).copy_into(next(target_attributes))
        except StopIteration:
            pass

    @property
    def cost(self) -> int:
        return sum(map(lambda x: x.count, self.all))

    @property
    def is_valid(self) -> bool:
        return all(map(lambda x: x.is_valid, self.all))

class State:
    def __init__(self) -> None:
        self.hacks = Hacks()
        self.skills = Skills()
        self.upgrades = Upgrades()

    @classmethod
    def copy(cls, source: 'State') -> 'State':
        target = State()
        source.copy_into(target)
        return target

    def copy_into(self, state: 'State') -> None:
        self.hacks.copy_into(state.hacks)
        self.skills.copy_into(state.skills)
        self.upgrades.copy_into(state.upgrades)

    @property
    def is_valid(self) -> bool:
        return all([
            self.hacks.is_valid,
            self.skills.is_valid,
            self.upgrades.is_valid,
            self.hacks.build_infection.count == 0 or self.skills.infect_payload.count == 0,
        ])

class Target:
    def __init__(self, name: str, breach_ratio: float) -> None:
        self.name = name
        self.breach_ratio = breach_ratio
        self.total_damage: float

class Targets:
    def __init__(self) -> None:
        self.anonymous = Target('Anonymous', 0.90)
        self.corporations = Target('Corporations', 0.80)
        self.government = Target('Government', 0.50)
        self.bank = Target('Bank', 0.50)

    @property
    def all(self) -> Iterator[Target]:
        yield self.anonymous
        yield self.corporations
        yield self.government
        yield self.bank

class Calculator:
    binomials: List[Tuple[int, ...]] = [(1,)]

    def __init__(self, state: State) -> None:
        if not state.is_valid: raise ValueError('State is invalid.')
        self.state = state

        self.payload_damage_per_attack = state.hacks.launch_payload.count / state.hacks.count
        self.payload_damage_per_attack *= state.upgrades.payload_damage.linear(1.00, 0.10)
        self.payload_damage_per_attack *= state.upgrades.infection_specialist.linear(1.00, -0.40)
        self.payload_damage_per_attack *= self._calculate_average_critical(state)

        self.infection_damage_per_attack = state.hacks.build_infection.count / state.hacks.count
        self.infection_damage_per_attack += state.skills.infect_payload.count * self.payload_damage_per_attack * 0.20
        self.infection_damage_per_attack *= state.upgrades.infection_damage.linear(1.00, 0.10)
        self.infection_damage_per_attack *= state.upgrades.infection_specialist.linear(1.00, 1.00)

        self.injection_multiplier = state.hacks.execute_injection.count * self._calculate_injection_multiplier(state)
        self.total_damage_per_attack = (self.payload_damage_per_attack + self.infection_damage_per_attack) * self.injection_multiplier

        self.total_seconds = state.upgrades.sneak_attack.linear(5, 1)
        self.attacks_per_second = state.upgrades.breach_speed.linear(1, 0.04)
        self.attacks = math.floor(self.total_seconds * self.attacks_per_second)
        self.total_damage = self.total_damage_per_attack * self.attacks
        if state.skills.ghost_attack.count > 0: self.total_damage *= 0.75

        self.targets = Targets()
        for target in self.targets.all:
            target_damage = self.total_damage
            if state.skills.ghost_attack.count > 0: target_damage /= target.breach_ratio
            target.total_damage = target_damage

    def _calculate_average_critical(self, state: State) -> float:
        max_hits = state.skills.critical_payload.count
        hit_chance = state.upgrades.critical_chance.linear(0.10, 0.02)
        miss_chance = 1 - hit_chance
        multiplier = state.upgrades.critical_multiplier.linear(2.50, 0.12)
        binomial = self._get_binomial_distribution(max_hits)
        damages = [
            binomial[hits] * (miss_chance ** (max_hits - hits)) * (hit_chance ** hits) * (multiplier ** hits)
            for hits in range(len(binomial))
        ]
        return sum(damages) / len(damages)

    def _calculate_injection_multiplier(self, state: State) -> float:
        return [
            1,
            2.75,
            2.68,
            2.64,
        ][state.hacks.count]
    
    def _get_binomial_distribution(self, attempts: int) -> Tuple[int, ...]:
        while attempts >= len(Calculator.binomials):
            n = len(Calculator.binomials)
            values = Calculator.binomials[-1]
            Calculator.binomials.append((
                values[0],
                *(
                    values[i - 1] + values[i]
                    for i in range(1, n)
                ),
                values[-1],
            ))

        return Calculator.binomials[attempts]

class Solver:
    def buy_skills(self, base_skills: Skills, budget: int) -> Iterator[Skills]:
        if budget <= 0:
            yield base_skills
            return
        
        for base_skill in base_skills.all:
            if base_skill.count >= base_skill.max: continue
            new_skills = Skills.copy(base_skills)
            new_skill = next(new_skill for new_skill in new_skills.all if new_skill.name == base_skill.name)
            new_skill.count += 1
            yield from self.buy_skills(new_skills, budget - 1)

    def buy_upgrades(self, base_upgrades: Upgrades, budget: int) -> Iterator[Upgrades]:
         