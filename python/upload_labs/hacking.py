#!/usr/bin/env python3

import math
from itertools import product
from typing import Any, Callable, Iterator, List, Optional, Tuple

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
        target_attributes = target.all
        for source_attribute in self.all:
            source_attribute.copy_into(next(target_attributes))

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
        self.critical_payload = Thing('Critical Payload')
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
        target_attributes = target.all
        for source_attribute in self.all:
            source_attribute.copy_into(next(target_attributes))

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
        target_attributes = target.all
        for source_attribute in self.all:
            source_attribute.copy_into(next(target_attributes))

    @property
    def cost(self) -> int:
        return sum(map(lambda x: x.count, self.all))

    @property
    def is_valid(self) -> bool:
        return all(map(lambda x: x.is_valid, self.all))

class State:
    def __init__(self,
        hacks: Optional[Hacks] = None,
        skills: Optional[Skills] = None,
        upgrades: Optional[Upgrades] = None,
    ) -> None:
        self.hacks = hacks or Hacks()
        self.skills = skills or Skills()
        self.upgrades = upgrades or Upgrades()

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
            not (self.hacks.launch_payload.count == 0 and self.skills.infect_payload.count > 0),
            not (self.hacks.launch_payload.count == 0 and self.skills.critical_payload.count > 0),
            not (self.hacks.build_infection.count > 0 and self.skills.infect_payload.count > 0),
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
        #self.bank = Target('Bank', 0.50)

    @property
    def all(self) -> Iterator[Target]:
        yield self.anonymous
        yield self.corporations
        yield self.government
        #yield self.bank

    @property
    def total_damage(self) -> float:
        return sum(map(lambda x: x.total_damage, self.all))

class Calculator:
    binomials: List[Tuple[int, ...]] = [(1,)]

    def __init__(self, state: State) -> None:
        if not state.is_valid: raise ValueError('State is invalid.')
        self.state = state

        self.total_seconds = state.upgrades.sneak_attack.linear(5, 1)
        self.attacks_per_second = state.upgrades.breach_speed.linear(1, 0.04)
        self.attacks = math.floor(self.total_seconds * self.attacks_per_second)

        self.payload_damage_per_attack = state.hacks.launch_payload.count / state.hacks.count
        self.payload_damage_per_attack *= state.upgrades.payload_damage.linear(1.00, 0.10)
        self.payload_damage_per_attack *= state.upgrades.infection_specialist.linear(1.00, -0.40)
        self.payload_damage_per_attack *= self._calculate_average_critical(state)
        self.payload_damage = self.payload_damage_per_attack * self.attacks

        self.infection_damage_per_attack = state.hacks.build_infection.count * 0.50 / state.hacks.count
        self.infection_damage_per_attack += state.skills.infect_payload.count * self.payload_damage_per_attack * 0.20
        self.infection_damage_per_attack *= state.upgrades.infection_damage.linear(1.00, 0.10)
        self.infection_damage_per_attack *= state.upgrades.infection_specialist.linear(1.00, 1.00)
        self.infection_duration = sum(
            self.total_seconds - attack / self.attacks_per_second
            for attack in range(self.attacks)
        )
        self.infection_damage = self.infection_damage_per_attack * self.infection_duration

        self.injection_multiplier = 1.0 if state.hacks.execute_injection.count == 0 else self._calculate_injection_multiplier(state)
        self.total_damage = (self.payload_damage + self.infection_damage) * self.injection_multiplier
        if state.skills.ghost_attack.count > 0: self.total_damage *= 0.75

        self.targets = Targets()
        for target in self.targets.all:
            target_damage = self.total_damage
            if state.skills.ghost_attack.count > 0: target_damage /= target.breach_ratio
            target.total_damage = target_damage

    def _calculate_average_critical(self, state: State) -> float:
        attempts = state.skills.critical_payload.count
        hit_chance = state.upgrades.critical_chance.linear(0.10, 0.02)
        miss_chance = 1 - hit_chance
        multiplier = state.upgrades.critical_multiplier.linear(2.50, 0.12)
        binomial = self._get_binomial_distribution(attempts)
        damages = [
            binomial[hits] * (miss_chance ** (attempts - hits)) * (hit_chance ** hits) * (multiplier ** hits)
            for hits in range(len(binomial))
        ]
        return sum(damages)

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

def buy_hacks() -> Iterator[Hacks]:
    def create(launch_payload: int, build_infection: int, execute_injection: int) -> Hacks:
        hacks = Hacks()
        hacks.launch_payload.count = launch_payload
        hacks.build_infection.count = build_infection
        hacks.execute_injection.count = execute_injection
        return hacks
    yield create(1, 0, 0)
    yield create(0, 1, 0)
    yield create(1, 1, 0)
    yield create(1, 0, 1)
    yield create(0, 1, 1)
    yield create(1, 1, 1)

def buy_skills(budget: int) -> Iterator[Skills]:
    def create(infect_payload: int, critical_payload: int, ghost_attack: int) -> Skills:
        skills = Skills()
        skills.infect_payload.count = infect_payload
        skills.critical_payload.count = critical_payload
        skills.ghost_attack.count = ghost_attack
        return skills
    if budget == 0:
        yield create(0, 0, 0)
    elif budget == 1:
        yield create(0, 0, 0)
        yield create(1, 0, 0)
        yield create(0, 0, 1)
        yield create(0, 1, 0)
    elif budget == 2:
        yield create(0, 0, 0)
        yield create(1, 0, 0)
        yield create(0, 0, 1)
        yield create(1, 0, 1)
        yield create(1, 1, 0)
        yield create(0, 1, 1)
        yield create(0, 2, 0)
    else:
        yield create(0, 0, 0)
        yield create(1, 0, 0)
        yield create(0, 0, 1)
        yield create(1, 0, 1)
        yield create(1, budget - 1, 0)
        yield create(0, budget - 1, 1)
        yield create(1, budget - 2, 1)
        yield create(0, budget, 0)

def buy_upgrades(upgrades: Upgrades, budget: int, skip: int = 0) -> Iterator[Upgrades]:
    if budget == 0:
        yield upgrades
        return
    base_upgrades = list(upgrades.all)
    for i in range(skip, len(base_upgrades)):
            base_upgrade = base_upgrades[i]
            if base_upgrade.count >= base_upgrade.max or budget < base_upgrade.cost: continue
            new_upgrades = Upgrades.copy(upgrades)
            new_upgrade = list(new_upgrades.all)[i]
            new_upgrade.count += 1
            yield from buy_upgrades(new_upgrades, budget - new_upgrade.cost, i)

def find_best(calculations: List[Calculator], *, key: Callable[[Calculator], Any]) -> List[Calculator]:
    best = [calculations[0]]
    best_score = key(best[0])
    for calculation in calculations[1:]:
        score = key(calculation)
        if score == best_score:
            best.append(calculation)
        elif score > best_score:
            best = [calculation]
            best_score = score
    return best

def report(title: str, calculations: List[Calculator]) -> None:
    calculation = calculations[0]
    state = calculation.state
    print(f'{title} (1 of {len(calculations)})')
    print('Hacks:')
    for hack in state.hacks.all:
        print(f'  {hack.count} {hack.name}')
    print('Skills:')
    for skill in state.skills.all:
        print(f'  {skill.count} {skill.name}')
    print('Upgrades:')
    for upgrade in state.upgrades.all:
        print(f'  {upgrade.count} {upgrade.name}')
    print(f'Total Damage: {calculation.targets.total_damage:.2f}')
    for target in calculation.targets.all:
        print(f'  {target.name} damage: {target.total_damage:.2f}')
    print()


base_state = State()
base_state.upgrades.payload_damage.count = 10
base_state.upgrades.breach_speed.count = 0
base_state.upgrades.critical_chance.count = 10
base_state.upgrades.critical_multiplier.count = 1
base_state.upgrades.infection_damage.count = 0
base_state.upgrades.infection_specialist.count = 0
base_state.upgrades.sneak_attack.count = 2

if True:
    hacks = list(buy_hacks())
    skills = list(buy_skills(5))
    upgrades = list(buy_upgrades(base_state.upgrades, 4))
    states = (State(*x) for x in product(hacks, skills, upgrades))
    calculations = [Calculator(state) for state in states if state.is_valid]
    best_overall = find_best(calculations, key=lambda x: x.targets.total_damage)
    report('Best Overall', best_overall)
    best_anonymous = find_best(calculations, key=lambda x: x.targets.anonymous.total_damage)
    if best_anonymous[0] is not best_overall[0]:
        report('Best Anonymous', best_anonymous)